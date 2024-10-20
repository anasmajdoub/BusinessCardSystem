import { Routes } from '@angular/router';
import { IndexBusinessCardComponent } from './businesscards/index-business-card/index-business-card.component';
import { CreateBusinessCardComponent } from './businesscards/create-business-card/create-business-card.component';

export const appRouts:Routes=[
    {path:'',redirectTo:'business-cards/Index',pathMatch:'full'},
    {path:'business-cards/Index',component:IndexBusinessCardComponent},
    {path:'business-cards/Create',component:CreateBusinessCardComponent}
]


export const routes: Routes = [

    { path: '', redirectTo: 'business-cards/Index', pathMatch: 'full' },

    { path: 'business-cards/Index', component: IndexBusinessCardComponent },
    
    { path: 'business-cards/Create', component: CreateBusinessCardComponent }
  ];
