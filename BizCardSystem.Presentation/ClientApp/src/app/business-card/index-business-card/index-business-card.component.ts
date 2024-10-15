import { Component, ViewChild } from '@angular/core';
import { BusinessCardService } from 'src/app/Services/business-card.service';
import { BusinessCard, BusinessCardFilter } from '../modules/businessCard';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { firstValueFrom } from 'rxjs';
import { PopupService } from 'src/app/Shared/popup/popup.service';
import { DownloadFile, FileTypes, getTimeStamp } from 'src/app/Shared/helpers';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DATE_FORMATS, MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule } from '@angular/material/paginator';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Gender } from 'src/app/Shared/Core/Enums/Gender';

@Component({
  selector: 'app-index-business-card',
  templateUrl: './index-business-card.component.html',
  styleUrls: ['./index-business-card.component.css']
})
export class IndexBusinessCardComponent {

  filter:BusinessCardFilter=new BusinessCardFilter();
  displayedColumns:string[]=["id","name","gender","dateofBirth","email","phone","photo","address","actions"]
  totalRecord: number = 0;
  dataSource = new MatTableDataSource<BusinessCard>();
  pageSizeOptions = [5, 10, 20, 50];  
  filterForm: FormGroup;
  genderOptions = Object.keys(Gender).filter(key => isNaN(Number(key)));

  constructor(private businessCardService:BusinessCardService,private popupService:PopupService,private formBuilder: FormBuilder) {
    this.filterForm = this.formBuilder.group({
      name: [''],
      gender: [''],
      dateOfBirth: [''],
      email: [''],
      phone: ['']
    });
  }
  
  ngOnInit(): void {
    this.GetAllBusinessCards();
  }

  GetAllBusinessCards(){
    this.businessCardService.GetAll(this.filter).subscribe(result=>{
      this.dataSource.data = result.values;
      this.totalRecord = result.totalRecord;
    });
  }

  applyFilter(){
    const rawDate = this.filterForm.get('dateOfBirth')?.value;
    
    this.filter.name = this.filterForm.get('name')?.value;
    this.filter.gender = this.filterForm.get('gender')?.value;
    this.filter.dateOfBirth = `${rawDate.getFullYear()}-${(rawDate.getMonth() + 1).toString().padStart(2, '0')}-${rawDate.getDate().toString().padStart(2, '0')}`;
    this.filter.email = this.filterForm.get('email')?.value;
    this.filter.phone = this.filterForm.get('phone')?.value;
    this.GetAllBusinessCards();
  }
  onPageChange(event: PageEvent) {
    this.filter.pageSize = event.pageSize;
    this.filter.pageIndex = event.pageIndex;
    this.GetAllBusinessCards();
  }
  async delete(id: number) {
    await  this.popupService.DeleteConfirm();
    this.businessCardService.Delete(id).subscribe(async result=>{
      if(result){
        await  this.popupService.OneButtonPopup("Deleted successfully","Confirm");
        this.GetAllBusinessCards();
      }
    })
  }
  exportToCSV(id:number) {
    this.businessCardService.ExportToCSV(id).subscribe(result=>{
    DownloadFile(result,FileTypes.csv.Type!,FileTypes.csv.Extension!);  
    })
  }
 
  exportToXML(id: number) {
    this.businessCardService.ExportToXML(id).subscribe(result=>{
      DownloadFile(result,FileTypes.xml.Type!,FileTypes.xml.Extension!);
     })
  }
clearFilter() {
  if (this.filterForm.dirty || this.filterForm.touched) {
    this.filterForm.reset();
    this.filter = new BusinessCardFilter();
    this.GetAllBusinessCards();
  }
}
}
