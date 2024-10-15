import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BusinessCardService } from 'src/app/Services/business-card.service';
import { Gender } from 'src/app/Shared/Core/Enums/Gender';
import { BusinessCardRequset } from '../modules/businessCard';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-business-card',
  templateUrl: './create-business-card.component.html',
  styleUrls: ['./create-business-card.component.css']
})
export class CreateBusinessCardComponent {
[x: string]: any;
  businessCardForm: FormGroup;
  selectedFile: File | null = null;
  previewData: any = null;
  genderOptions = Object.keys(Gender).filter(key => isNaN(Number(key)));
  constructor(
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private businessCardService:BusinessCardService,
    private router:Router,
  ) {

    this.businessCardForm = this.formBuilder.group({
      name: ['', Validators.required],
      gender: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      country: [''],
      city: ['',],
      state: ['',],
      zipCode: [''],
      street: [''],
      photo: [null]
    });
  }

  ngOnInit() {
    this.businessCardForm.valueChanges.subscribe(formData => {
      this.previewData = { ...formData };
      this.previewData.address=`${formData.street}, ${formData.city}, ${formData.state}, ${formData.zipCode}, ${formData.country}`; 
    });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file && file.size <= 1000000) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.businessCardForm.patchValue({
          photo: e.target.result
        });
      };
      reader.readAsDataURL(file);
    } else {
      this.snackBar.open('File size must be 1MB or less', 'Close', { duration: 3000 });
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.handleImportFile(files[0]);
    }
  }

  onImportFileSelected(event: any) {
    const file: File = event.target.files[0];
    this.handleImportFile(file);
  }

  handleImportFile(file: File) {
    
    this.businessCardService.uploadFile(file).subscribe(()=>{
      this.snackBar.open(`Imported file: ${file.name}`, 'Close', { duration: 3000 });
    });
  }

  onSubmit() {
 
    if (this.businessCardForm.valid) 
      {
        const businessCardRequest: BusinessCardRequset = {
          name: this.businessCardForm.value.name,
          gender:this.businessCardForm.value.gender as Gender,
          dateOfBirth: this.businessCardForm.value.dateOfBirth,
          email: this.businessCardForm.value.email,
          phone: this.businessCardForm.value.phone,
          photo: this.businessCardForm.value.photo,
          address: {
            street: this.businessCardForm.value.street,
            city: this.businessCardForm.value.city,
            state: this.businessCardForm.value.state,
            zipCode: this.businessCardForm.value.zipCode,
            country: this.businessCardForm.value.country
          }
        };
        this.businessCardService.Create(businessCardRequest).subscribe(result => {
          if (result) {
            this.snackBar.open('Business card submitted successfully!', 'Close', { duration: 3000 });
            this.router.navigate(['/Index']);
          }
        });
     
    }
  }
}
