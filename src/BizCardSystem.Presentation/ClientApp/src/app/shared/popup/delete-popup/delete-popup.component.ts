import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MaterialModule } from '../../material/material.module';
 
@Component({
  selector: 'app-delete-popup',
  templateUrl: './delete-popup.component.html',
  styleUrls: ['./delete-popup.component.css'],
  standalone:true,
  imports:[MaterialModule]
})
export class DeletePopupComponent {
  constructor(
    public dialogRef: MatDialogRef<DeletePopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  confirm() {
    this.dialogRef.close(true);
  }

  dismiss() {
    this.dialogRef.close(false);
  }
}
