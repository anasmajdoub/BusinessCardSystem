import { ChangeDetectorRef, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { MaterialModule } from '../../material/material.module';
import { BarcodeFormat } from '@zxing/library';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
@Component({
  selector: 'app-qr-code-imporer',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './qr-code-imporer.component.html',
  styleUrl: './qr-code-imporer.component.css'
})
export class QrCodeImporerComponent {
  @ViewChild('scanner') scanner: ZXingScannerComponent | undefined;
  @Output() close = new EventEmitter<void>();
  @Output() scanned = new EventEmitter<string>();

  availableDevices: MediaDeviceInfo[] = [];
  selectedDevice: MediaDeviceInfo | undefined;
  allowedFormats = [BarcodeFormat.QR_CODE];
  errorMessage: string | null = null;

  constructor(private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    console.log('QR Code Importer Component initialized');
  }

  ngOnDestroy() {
    console.log('QR Code Importer Component destroyed');
  }

  camerasFound(devices: MediaDeviceInfo[]): void {
    console.log('Cameras found:', devices);
    this.availableDevices = devices;
    this.selectedDevice = devices.find(device => device.label.toLowerCase().includes('back')) || devices[0];
    this.cdr.detectChanges();
  }

  camerasNotFound(event: any): void {
    this.errorMessage = 'No cameras found. Please make sure your device has a camera and it\'s not being used by another application.';
    this.cdr.detectChanges();
  }

  onDeviceSelect(event: Event): void {
    const deviceId = (event.target as HTMLSelectElement).value;
    this.selectedDevice = this.availableDevices.find(device => device.deviceId === deviceId);
    this.cdr.detectChanges();
  }

  onPermissionResponse(permission: boolean): void {
    if (!permission) {
      this.errorMessage = 'Camera permission denied. Please grant permission to use the camera.';
      this.cdr.detectChanges();
    }
  }

  onQrCodeScanned(data: string) {
    this.scanned.emit(data);
    this.closeModal();
  }

  onScanFailure(event: any): void {
  }

  closeModal() {
    this.close.emit();
  }
}
