import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-one-button-popup',
  templateUrl: './one-button-popup.component.html',
  styleUrls: ['./one-button-popup.component.css']
})
export class OneButtonPopupComponent {
  constructor(public activeModal: NgbActiveModal) {}

  @Input() message!: string; 
  @Input() buttonText!: string; 
  confirm() {
    this.activeModal.close(true);
  }

  dismiss() {
    this.activeModal.dismiss(false);
  }
}
