import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexBusinessCardComponent } from './business-card/index-business-card/index-business-card.component';
import { CreateBusinessCardComponent } from './business-card/create-business-card/create-business-card.component';

const routes: Routes = [
  { path: 'create', component: CreateBusinessCardComponent },
  { path: 'Index', component: IndexBusinessCardComponent },
  { path: '', redirectTo: 'Index', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
