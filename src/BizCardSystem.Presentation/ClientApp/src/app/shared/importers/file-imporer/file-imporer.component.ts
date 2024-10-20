import { Component, ElementRef, EventEmitter, Output, ViewChild } from '@angular/core';
import { MaterialModule } from '../../material/material.module';
import { BusinessCardService } from '../../services/business-card.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApiClient } from '../../Api/api-client.service';
import { BusinessCard } from '../../../modules/businesscards/BusinessCard';
import { BusinessCardRequset } from '../../../modules/businesscards/BusinessCardRequset';
import { BrowserMultiFormatReader, NotFoundException, Result } from '@zxing/library';

@Component({
  selector: 'app-file-imporer',
  standalone: true,
  imports: [MaterialModule,HttpClientModule],
  templateUrl: './file-imporer.component.html',
  styleUrl: './file-imporer.component.css',
  providers: [BusinessCardService,ApiClient,HttpClient]
})
export class FileImporerComponent {
  @Output() close = new EventEmitter<void>();
  @Output() businessCard = new EventEmitter<BusinessCardRequset>();
  @ViewChild('fileInput') fileInput: ElementRef<HTMLInputElement> | undefined; // Explicitly define the type
  selectedFile: File | null = null;
  selectedFileName: string = '';  
  fileError: string = '';
  constructor(private businessCardService:BusinessCardService) 
  {

  }
  onFileDrop(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.selectedFileName = this.selectedFile.name; 
      this.fileError = ''; 
    }
  }


  onSubmitDragDrop(){
    if (!this.selectedFile) {
      this.fileError = 'No file selected!';
      return;
    }
    if (
      this.selectedFile.type !== 'text/csv' &&
      this.selectedFile.type !== 'application/xml' &&
      this.selectedFile.type !== 'text/xml' &&
      this.selectedFile.type !== 'image/jpeg' &&
      this.selectedFile.type !== 'image/png' &&
      this.selectedFile.type !== 'image/gif' &&
      this.selectedFile.type !== 'image/jpg'
    ){      this.fileError = 'Invalid file type. Only CSV or XML or Qr Code Image files are allowed.';
      return;
    }


    this.businessCardService.uploadFile(this.selectedFile).subscribe(result=>{
      if(result.isSuccess){
        this.businessCard.emit(result.value);
        this.closeModal();
      }else{
        this.fileError = 'Error happened try again'
      }
     
    })
  }

  closeModal() {
    this.clearFileInput();
    this.close.emit();
  }

  clearFileInput() {
    this.selectedFile = null;
    this.fileError = '';
    this.selectedFileName =''; 
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
    }
  }
}
