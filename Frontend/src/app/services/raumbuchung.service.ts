import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

/* =======================
   INTERFACES
======================= */
export interface Raum {
  raumId: number;
  raumName: string;
  kapazitaet: number;
  ausstattung?: string;
  etage?: number;
  gebaeude?: string;
  aktiv: boolean;
}

export interface Buchung {
  buchungId: number;
  raumId: number;
  benutzerName: string;
  benutzerEmail: string;
  startZeit: string;
  endZeit: string;
  buchungsZweck: string;
  teilnehmerAnzahl?: number;
  status: string;
  bemerkungen?: string;
  raum?: Raum;
}

export interface CreateBuchung {
  raumId: number;
  benutzerName: string;
  benutzerEmail: string;
  startZeit: string;
  endZeit: string;
  buchungsZweck: string;
  teilnehmerAnzahl?: number;
  bemerkungen?: string;
}

@Injectable({
  providedIn: 'root'
})
export class RaumbuchungService {

  private apiUrl = 'http://localhost:5012/api';

  constructor(private http: HttpClient) {}

  /* =======================
     RÄUME
  ======================= */
  getRaume(): Observable<Raum[]> {
    return this.http.get<Raum[]>(`${this.apiUrl}/raume`).pipe(
      catchError(() => {
        console.warn('Backend nicht erreichbar – Test-Daten werden verwendet');

        // ✅ TESTDATEN
        return of<Raum[]>([
          {
            raumId: 1,
            raumName: 'Konferenzraum A',
            kapazitaet: 12,
            aktiv: true
          },
          {
            raumId: 2,
            raumName: 'Meetingraum B',
            kapazitaet: 6,
            aktiv: true
          },
          {
            raumId: 3,
            raumName: 'Schulungsraum C',
            kapazitaet: 20,
            aktiv: true
          },
          {
            raumId: 4,
            raumName: 'Kreativraum D',
            kapazitaet: 8,
            aktiv: true
          }
        ]);
      })
    );
  }

  getRaumById(id: number): Observable<Raum> {
    return this.http.get<Raum>(`${this.apiUrl}/raume/${id}`);
  }

  /* =======================
     BUCHUNGEN
  ======================= */
  getBuchungen(): Observable<Buchung[]> {
    return this.http.get<Buchung[]>(`${this.apiUrl}/buchungen`);
  }

  getBuchungById(id: number): Observable<Buchung> {
    return this.http.get<Buchung>(`${this.apiUrl}/buchungen/${id}`);
  }

  createBuchung(buchung: CreateBuchung): Observable<Buchung> {
    return this.http.post<Buchung>(`${this.apiUrl}/buchungen`, buchung);
  }

  getBuchungenByRaum(raumId: number): Observable<Buchung[]> {
    return this.http.get<Buchung[]>(`${this.apiUrl}/buchungen/raum/${raumId}`);
  }

  checkVerfuegbarkeit(
    raumId: number,
    startZeit: string,
    endZeit: string
  ): Observable<boolean> {
    const params = new HttpParams()
      .set('raumId', raumId)
      .set('startZeit', startZeit)
      .set('endZeit', endZeit);

    return this.http.get<boolean>(
      `${this.apiUrl}/buchungen/verfuegbarkeit`,
      { params }
    );
  }
}
