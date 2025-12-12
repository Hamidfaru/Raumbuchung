import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RaumbuchungService, Raum } from '../../services/raumbuchung.service';

@Component({
  selector: 'app-raum-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './raum-list.component.html',
  styleUrl: './raum-list.component.css'
})
export class RaumListComponent implements OnInit {
  raume: Raum[] = [];
  loading = true;
  error = '';

    categories = [
    {
      id: 1,
      name: 'Konferenzräume',
      description: 'Professionelle Räume für Besprechungen und Präsentationen',
      icon: '💼',
      count: 4
    },
    {
      id: 2, 
      name: 'Meetingräume',
      description: 'Kompakte Räume für Team-Meetings',
      icon: '👥',
      count: 6
    },
    {
      id: 3,
      name: 'Schulungsräume', 
      description: 'Räume für Workshops und Schulungen',
      icon: '🎓',
      count: 3
    },
    {
      id: 4,
      name: 'Kreativräume',
      description: 'Inspirierende Räume für Brainstorming',
      icon: '💡',
      count: 2
    },
    {
      id: 5,
      name: 'Eventräume',
      description: 'Große Räume für Veranstaltungen',
      icon: '🎉',
      count: 2

    }
  ];


  constructor(private raumbuchungService: RaumbuchungService) { }

  ngOnInit(): void {
    this.loadRaume();
  }

  loadRaume(): void {
  this.loading = true;
  
  // ECHTER Service (Test-Daten auskommentieren)
  this.raumbuchungService.getRaume().subscribe({
    next: (data) => {
      this.raume = data;
      this.loading = false;
    },
    error: (error) => {
      console.error('API Error:', error);
      // Fallback: Test-Daten wenn Backend nicht erreichbar
      this.raume = [
        {
          raumId: 1,
          raumName: 'Konferenzraum A',
          kapazitaet: 12,
          ausstattung: 'Beamer, Whiteboard, Telefon',
          etage: 1,
          gebaeude: 'Hauptgebäude',
          aktiv: true
        },
        {
          raumId: 2, 
          raumName: 'Meetingraum B',
          kapazitaet: 6,
          ausstattung: 'Monitor, Whiteboard',
          etage: 2,
          gebaeude: 'Hauptgebäude', 
          aktiv: true
        }
      ];
      this.loading = false;
      this.error = 'Backend nicht erreichbar - Zeige Test-Daten';
    }
  });

  // Test-Daten auskommentieren:
  // setTimeout(() => { ... }, 1500);
}

  getGradient(raumId: number): string {
    const gradients = [
      'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)', 
      'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
      'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)'
    ];
    return gradients[(raumId - 1) % gradients.length];
  }
}