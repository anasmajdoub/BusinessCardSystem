import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeletePopupComponent } from './delete-popup/delete-popup.component';
import { OneButtonPopupComponent } from './one-button-popup/one-button-popup.component';

@Injectable({
  providedIn: 'root'
})
export class PopupService {

  constructor(private dialog: MatDialog) { }

  async DeleteConfirm(): Promise<boolean> {
    const dialogRef = this.dialog.open(DeletePopupComponent, {
      width: '400px',
      disableClose: true,
      panelClass: 'custom-dialog',
      data: { centered: true }
    });
    return await dialogRef.afterClosed().toPromise();
  }

  async OneButtonPopup(message: string, buttonText = "Confirm"): Promise<boolean> {
    const dialogRef = this.dialog.open(OneButtonPopupComponent, {
      width: '400px',
      disableClose: true,
      panelClass: 'custom-dialog',
      data: { message, buttonText }
    });
    return await dialogRef.afterClosed().toPromise();
  }
}
