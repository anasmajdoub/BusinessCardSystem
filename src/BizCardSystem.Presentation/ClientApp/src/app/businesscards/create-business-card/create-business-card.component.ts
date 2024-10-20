import { Component } from '@angular/core';
import { MaterialModule } from '../../shared/material/material.module';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Gender } from '../../modules/enums/Gender';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { addressValidator, businessCardValidator } from '../Validators';
import { BusinessCardService } from '../../shared/services/business-card.service';
import { BusinessCardRequset } from '../../modules/businesscards/BusinessCardRequset';
import { Router } from '@angular/router';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ApiClient } from '../../shared/Api/api-client.service';
@Component({
  selector: 'app-create-business-card',
  standalone: true,
  imports: [MaterialModule,HttpClientModule],
  templateUrl: './create-business-card.component.html',
  styleUrl: './create-business-card.component.css',
  providers: [BusinessCardService,ApiClient,HttpClient]
})
export class CreateBusinessCardComponent {
  businessCardForm: FormGroup;
  genders = Object.keys(Gender).filter(key => isNaN(Number(key)));
  previewData: any = null;
  isFormSubmitted = false;
  

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
        this.businessCardForm.patchValue({ photo: reader.result });
        this.businessCardForm.get('photo')?.updateValueAndValidity();
      };
      reader.readAsDataURL(file);
    }
  }

  onDrop(event: CdkDragDrop<any[]>) {
    console.log('Dropped files:', event);
    // Handle drag-and-drop processing logic here
  }

  onSubmit() {
    console.log(this.businessCardForm.errors);
    if (this.businessCardForm.valid) {
      this.isFormSubmitted = true;
      this.previewData = this.businessCardForm.value;
    }
  }

  onSave() {
    console.log( this.businessCardForm.get('gender')?.value as number);
    const businessCardRequest: BusinessCardRequset = {
      name: this.businessCardForm.get('name')?.value,
      gender: this.businessCardForm.get('gender')?.value as number,
      dateOfBirth: this.businessCardForm.get('dateOfBirth')?.value,
      email: this.businessCardForm.get('email')?.value,
      phone: this.businessCardForm.get('phone')?.value,
      photo: this.businessCardForm.get('photo')?.value,
      address: {
        street: this.businessCardForm.get('address.street')?.value,
        city: this.businessCardForm.get('address.city')?.value,
        state: this.businessCardForm.get('address.state')?.value,
        zipCode: this.businessCardForm.get('address.zipCode')?.value,
        country: this.businessCardForm.get('address.country')?.value
      }
    };
    console.log(businessCardRequest);
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
}
 
