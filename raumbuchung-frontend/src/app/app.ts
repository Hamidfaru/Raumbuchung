import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NavigationComponent } from './components/navigation/navigation.component';

@Component({
  selector: 'app-root',
  imports: [
    HttpClientModule, 
    RouterModule,
    NavigationComponent
    // RaumListComponent entfernen - wird über Routes geladen
  ],
  template: `
    <app-navigation></app-navigation>
    <router-outlet></router-outlet>
  `,
  styleUrl: './app.css'
})
export class App {
  protected readonly title = 'Raumbuchung';
}