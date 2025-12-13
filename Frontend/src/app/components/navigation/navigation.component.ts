import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent {
  isMenuActive = false;
  activeDropdown: string | null = null;

  toggleMenu(): void {
    this.isMenuActive = !this.isMenuActive;
  }

  closeMenu(): void {
    this.isMenuActive = false;
    this.activeDropdown = null;
  }

  toggleDropdown(dropdown: string): void {
    this.activeDropdown = this.activeDropdown === dropdown ? null : dropdown;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    // Schließe Dropdowns wenn außerhalb geklickt wird
    const target = event.target as HTMLElement;
    if (!target.closest('.dropdown')) {
      this.activeDropdown = null;
    }
  }
}