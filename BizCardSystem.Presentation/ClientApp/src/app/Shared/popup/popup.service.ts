import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DeletePopupComponent } from './delete-popup/delete-popup.component';
import { OneButtonPopupComponent } from './one-button-popup/one-button-popup.component';

@Injectable({
  providedIn: 'root'
})
export class PopupService {

  constructor(private modalService: NgbModal) { }

  async DeleteConfirm(): Promise<boolean> {
    const modalRef = this.modalService.open(DeletePopupComponent, { centered: true });
    return await modalRef.result;
  }

  async OneButtonPopup(message: string,buttonText="Confirm"): Promise<boolean> {
    const modalRef = this.modalService.open(OneButtonPopupComponent, { centered: true });
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.buttonText = buttonText;

    return await modalRef.result;
  }
}
