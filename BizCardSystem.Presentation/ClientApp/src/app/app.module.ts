import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { IndexBusinessCardComponent } from './business-card/index-business-card/index-business-card.component';
import { CreateBusinessCardComponent } from './business-card/create-business-card/create-business-card.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { BusinessCardService } from './Services/business-card.service';
import { ApiClient } from './Shared/Api/api-client.service';
import { HttpClientModule } from '@angular/common/http';
import { DeletePopupComponent } from './Shared/popup/delete-popup/delete-popup.component';
import { OneButtonPopupComponent } from './Shared/popup/one-button-popup/one-button-popup.component';
import { FormsModule } from '@angular/forms';
import { DragDropModule } from '@angular/cdk/drag-drop';
import {MatSnackBarModule} from '@angular/material/snack-bar';
 

@NgModule({
  declarations: [
    AppComponent,
    IndexBusinessCardComponent,
    CreateBusinessCardComponent,
    DeletePopupComponent,
    OneButtonPopupComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    DragDropModule,
    MatSnackBarModule
  ],
  exports:[],
  providers: [BusinessCardService,ApiClient],
  bootstrap: [AppComponent]
})
export class AppModule { }
