import { Component } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NavigationComponent } from './components/navigation/navigation.component';
import { FooterComponent } from './components/footer/footer.component';

@Component({
  selector: 'app-root',
  imports: [
    HttpClientModule, 
    RouterModule,
    NavigationComponent,
      FooterComponent 
    // RaumListComponent entfernen - wird über Routes geladen
  ],
  template: `
    <app-navigation></app-navigation>
    <router-outlet></router-outlet>
     <app-footer></app-footer>
  `,
  styleUrl: './app.css'
})
export class App {
  protected readonly title = 'Raumbuchung';


}

