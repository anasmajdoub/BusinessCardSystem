import { Component } from '@angular/core';
import { MaterialModule } from '../../shared/material/material.module';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Gender } from '../../modules/enums/Gender';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { addressValidator, businessCardValidator, validateBase64File } from '../Validators';
import { BusinessCardService } from '../../shared/services/business-card.service';
import { BusinessCardRequset } from '../../modules/businesscards/BusinessCardRequset';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApiClient } from '../../shared/Api/api-client.service';
import { FileImporerComponent } from "../../shared/importers/file-imporer/file-imporer.component";
import { QrCodeImporerComponent } from "../../shared/importers/qr-code-imporer/qr-code-imporer.component";
import { BusinessCard } from '../../modules/businesscards/BusinessCard';
@Component({
  selector: 'app-create-business-card',
  standalone: true,
  imports: [MaterialModule, HttpClientModule, FileImporerComponent, QrCodeImporerComponent],
  templateUrl: './create-business-card.component.html',
  styleUrl: './create-business-card.component.css',
  providers: [BusinessCardService,ApiClient,HttpClient]
})
export class CreateBusinessCardComponent {

  businessCardForm: FormGroup;
  genders = Object.keys(Gender).filter(key => isNaN(Number(key)));
  previewData: BusinessCardRequset| null=null;
  isFormSubmitted = false;
  showDragDropForm = false;
  showQrCodeForm = false;
  showBusinessCardForm = false;
  scannedData: string | null = null;

  constructor(private fb: FormBuilder,private businessCardService:BusinessCardService,private router:Router) {
    this.businessCardForm = this.fb.group({
      name: ['', Validators.required],
      gender: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      photo: [null, Validators.required],
      address: this.fb.group({
        street: [''],
        city: [''],
        state: [''],
        zipCode: [''],
        country: ['']
      }, { validators: addressValidator() })
    }, { validators: businessCardValidator() });
    
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file && file.type.startsWith('image/')) {
      const reader = new FileReader();
      reader.onload = () => {
        // this.businessCardForm.patchValue({ photo: reader.result });
        // this.businessCardForm.get('photo')?.updateValueAndValidity();

        const base64String = reader.result as string;

        // Validate base64 size and format before setting the value
        const errors = validateBase64File(base64String);

        if (!errors) {
          // Only update the form control if no errors are found
          this.businessCardForm.get('photo')?.setValue(base64String);
        } else {
          // Set errors manually without causing recursive revalidation
          this.businessCardForm.get('photo')?.setErrors(errors);
        }
      };
      reader.readAsDataURL(file);
    }
  }

  onSubmit() {

    if (this.businessCardForm.valid) {
      this.isFormSubmitted = true;
      this.previewData = this.businessCardForm.value;
    }else{
      this.businessCardForm.markAllAsTouched();  
      return; 
    }
  }

  onSave() {
    const businessCardRequest: BusinessCardRequset = {
      name: this.previewData?.name as string,
      gender: this.previewData?.gender as number,
      dateofBirth: this.previewData?.dateofBirth  as Date,
      email: this.previewData?.email as string,
      phone: this.previewData?.phone as string,
      photo: this.previewData?.photo as string,
      address: {
        street: this.previewData?.address.street as string,
        city: this.previewData?.address.city as string,
        state: this.previewData?.address.state as string,
        zipCode: this.previewData?.address.zipCode as string,
        country: this.previewData?.address.country as string
      }
    };
     this.businessCardService.Create(businessCardRequest).subscribe((result)=>{
           if(result){
            this.router.navigate(['/business-cards/Index']);
           }  
     });
  }

  onBack() {
    this.isFormSubmitted = false;
    this.previewData = null;
  }
  showDragDrop() {
    this.showDragDropForm = true;
    this.showQrCodeForm = false;
    this.showBusinessCardForm = false;
  }
  hideFileImporter() {
    this.showDragDropForm = false;
    this.showQrCodeForm = false;
    this.showBusinessCardForm = true;
    }
    hideQrCodeImporter()
    {
      this.showDragDropForm = false;
      this.showQrCodeForm = false;
      this.showBusinessCardForm = true;
    }
  showQrReader() {
    this.showQrCodeForm = true;
    this.showDragDropForm = false;
    this.showBusinessCardForm = false;
  }
  saveBusinessCardByFile(businessCard:BusinessCardRequset){
    this.hideFileImporter();
    this.isFormSubmitted = true;
    this.previewData = businessCard;
  }
  saveBusinessCardByQrCode(businessCard:string){
    const businessCardObj: BusinessCardRequset = JSON.parse(businessCard);
    this.hideFileImporter();
    this.isFormSubmitted = true;
    this.previewData = businessCardObj;
  }
}
 
