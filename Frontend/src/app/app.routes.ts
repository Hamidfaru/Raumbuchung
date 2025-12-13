import { Routes } from '@angular/router';
import { RaumListComponent } from './components/raum-list/raum-list.component';
import { BuchungFormComponent } from './components/buchung-form/buchung-form.component';
import { ErfolgComponent } from './components/erfolg/erfolg.component';

export const routes: Routes = [
  { path: '', component: RaumListComponent },
  { path: 'buchung/:id', component: BuchungFormComponent },
  { path: 'erfolg', component: ErfolgComponent },
  { path: '**', redirectTo: '' }
];