import { Component, Inject, Input } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MaterialModule } from '../../material/material.module';
 

@Component({
  selector: 'app-one-button-popup',
  templateUrl: './one-button-popup.component.html',
  styleUrls: ['./one-button-popup.component.css'],
  standalone:true,
  imports:[MaterialModule]
})
export class OneButtonPopupComponent {
  constructor(
    public dialogRef: MatDialogRef<OneButtonPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  confirm() {
    this.dialogRef.close(true);
  }

  dismiss() {
    this.dialogRef.close(false);
  }
}
