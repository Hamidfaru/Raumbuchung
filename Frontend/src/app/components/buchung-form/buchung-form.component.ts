import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RaumbuchungService, Raum, CreateBuchung } from '../../services/raumbuchung.service';

@Component({
  selector: 'app-buchung-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './buchung-form.component.html',
  styleUrl: './buchung-form.component.css'
})
export class BuchungFormComponent implements OnInit {
  buchungForm: FormGroup;
  raum?: Raum;
  loading = false;
  submitted = false;
  error = '';
  raumId: number;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private raumbuchungService: RaumbuchungService
  ) {
    this.raumId = Number(this.route.snapshot.paramMap.get('id'));
    
    this.buchungForm = this.formBuilder.group({
      benutzerName: ['', [Validators.required, Validators.minLength(2)]],
      benutzerEmail: ['', [Validators.required, Validators.email]],
      startZeit: ['', Validators.required],
      endZeit: ['', Validators.required],
      buchungsZweck: ['', [Validators.required, Validators.minLength(5)]],
      teilnehmerAnzahl: ['', [Validators.min(1)]],
      bemerkungen: ['']
    });
  }

  ngOnInit(): void {
    this.loadRaumDetails();
  }

  loadRaumDetails(): void {
    // TEMPORÄR: Test-Daten für Raum
    if (this.raumId === 1) {
      this.raum = {
        raumId: 1,
        raumName: 'Konferenzraum A',
        kapazitaet: 12,
        ausstattung: 'Beamer, Whiteboard, Telefon',
        etage: 1,
        gebaeude: 'Hauptgebäude',
        aktiv: true
      };
    } else if (this.raumId === 2) {
      this.raum = {
        raumId: 2,
        raumName: 'Meetingraum B',
        kapazitaet: 6,
        ausstattung: 'Monitor, Whiteboard',
        etage: 2,
        gebaeude: 'Hauptgebäude',
        aktiv: true
      };
    } else {
      // Fallback für andere Raum-IDs
      this.raum = {
        raumId: this.raumId,
        raumName: `Raum ${this.raumId}`,
        kapazitaet: 10,
        ausstattung: 'Standard',
        etage: 1,
        gebaeude: 'Hauptgebäude',
        aktiv: true
      };
    }
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.buchungForm.invalid) {
      return;
    }

    this.loading = true;
    
    const buchung: CreateBuchung = {
      raumId: this.raumId,
      ...this.buchungForm.value
    };

    // Datum/Zeit Format anpassen
    buchung.startZeit = new Date(buchung.startZeit).toISOString();
    buchung.endZeit = new Date(buchung.endZeit).toISOString();

    // TEMPORÄR: Erfolgs-Seite anzeigen statt echten API Call
    setTimeout(() => {
      this.loading = false;
      this.router.navigate(['/erfolg']);
    }, 1500);

    // Später: Echten API Call aktivieren
    // this.raumbuchungService.createBuchung(buchung).subscribe({
    //   next: () => {
    //     this.router.navigate(['/erfolg']);
    //   },
    //   error: (error) => {
    //     this.error = error.error || 'Fehler beim Erstellen der Buchung';
    //     this.loading = false;
    //   }
    // });
  }

  // Getter für einfacheren Zugriff auf Form-Felder
  get f() { 
    return this.buchungForm.controls; 
  }
}
