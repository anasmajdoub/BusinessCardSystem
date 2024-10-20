import { Component } from '@angular/core';
import { MaterialModule } from '../../shared/material/material.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormGroup, FormBuilder } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { BusinessCard } from '../../modules/businesscards/BusinessCard';
import { Gender } from '../../modules/enums/Gender';
import { BusinessCardFilter } from '../../modules/filter/BusinessCardFilter';
import { HeaderComponent } from '../../shared/header/header.component';
import { DownloadFile, FileTypes } from '../../shared/helper';
import { PopupService } from '../../shared/popup/popup.service';
import { BusinessCardService } from '../../shared/services/business-card.service';
import { ApiClient } from '../../shared/Api/api-client.service';
import { NotificationService } from '../../shared/services/notification.service';

@Component({
  selector: 'app-index-business-card',
  standalone: true,
  imports: [MaterialModule, HttpClientModule, HeaderComponent],
  templateUrl: './index-business-card.component.html',
  styleUrl: './index-business-card.component.css',
  providers: [BusinessCardService,ApiClient,HttpClient]
})
export class IndexBusinessCardComponent {
  businessCards: BusinessCard[] = [];
  businessCardsfilter: BusinessCardFilter = new BusinessCardFilter()
  options: FormGroup;
  genders = Object.keys(Gender).filter(key => isNaN(Number(key))); 
  isLoading = true;
  totalRecords = 0;
  
  constructor(private fb: FormBuilder,private businessCardService:BusinessCardService,private popupService:PopupService,
    private notificationService:NotificationService) {
    this.options = this.fb.group({
      name: [this.businessCardsfilter.name],            
      gender: [null],        
      dateOfBirth: [this.businessCardsfilter.dateOfBirth],  
      email: [this.businessCardsfilter.email],          
      phone: [this.businessCardsfilter.phone],           
      pageIndex: [this.businessCardsfilter.pageIndex],   
      pageSize: [this.businessCardsfilter.pageSize],    
      sortColumn: [this.businessCardsfilter.sortColumn],  
      sortDirection: [this.businessCardsfilter.sortDirection], 
    });
  }
    ngOnInit(): void
    {
  
      this.loadBusinessCards();
  
      this.options.valueChanges.subscribe((filterValues) => {
        this.businessCardsfilter = { ...this.businessCardsfilter, ...filterValues };
        this.loadBusinessCards();
      });
    }
  
    loadBusinessCards(): void {
      this.businessCardService.GetAll(this.businessCardsfilter).subscribe({
        next: (cards) => {
          this.businessCards = cards.values;
          this.isLoading = false;
          this.totalRecords= cards.totalRecord;
        },
        error: (err) => {
          console.error('Failed to load business cards', err);
          this.isLoading = false;
        }
      });
    }
    onPageChange(event: PageEvent) {
      this.businessCardsfilter.pageSize = event.pageSize;
      this.businessCardsfilter.pageIndex = event.pageIndex;
      this.loadBusinessCards();
    }
   
  
   formatAddress(address:string) {
      if (!address) {
        return '';
      }
      const addressParts = address.split(',').filter(part => part && part.trim() !== '');
      return addressParts.join(', ');
    }
  
  
  applyFilter(): void {
    this.loadBusinessCards();
  }
  resetFilter(): void {
    this.options.reset();
  }
  
  downloadCsv(id:number): void {
    this.businessCardService.ExportToCSV(id).subscribe(result=>{
      DownloadFile(result,FileTypes.csv.Type!,FileTypes.csv.Extension!);  
    });
  }
  downloadXml(id:number): void {
    this.businessCardService.ExportToXML(id).subscribe(result=>{
      DownloadFile(result,FileTypes.xml.Type!,FileTypes.xml.Extension!);
    });
   
  }
  
  async deleteCard(id:number): Promise<void> {
    await this.popupService.DeleteConfirm();
    this.businessCardService.Delete(id).subscribe((result)=>{
         if(result){
          this.notificationService.openSnackBar();
          this.loadBusinessCards();
         }
    });
  } 
}
