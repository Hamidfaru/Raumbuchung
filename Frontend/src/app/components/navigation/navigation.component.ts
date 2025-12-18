import { Component, HostListener, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
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
  isDark = false;
  private isBrowser: boolean;

  constructor(@Inject(PLATFORM_ID) platformId: object) {
    this.isBrowser = isPlatformBrowser(platformId);

    if (this.isBrowser) {
      const savedTheme = localStorage.getItem('theme');
      if (savedTheme === 'dark') {
        this.enableDark();
      }
    }
  }

  toggleMenu(): void {
    this.isMenuActive = !this.isMenuActive;
  }

  toggleDropdown(name: string): void {
    this.activeDropdown = this.activeDropdown === name ? null : name;
  }

  toggleTheme(): void {
    this.isDark ? this.enableLight() : this.enableDark();
  }

  enableDark(): void {
    if (!this.isBrowser) return;

    document.body.classList.add('dark');
    localStorage.setItem('theme', 'dark');
    this.isDark = true;
  }

  enableLight(): void {
    if (!this.isBrowser) return;

    document.body.classList.remove('dark');
    localStorage.setItem('theme', 'light');
    this.isDark = false;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.dropdown')) {
      this.activeDropdown = null;
    }
  }
}
