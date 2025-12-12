import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

// 👇 INTERFACES MÜSSEN OBEN DEFINIERIERT SEIN
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
  private apiUrl = 'https://localhost:7172/api';

  constructor(private http: HttpClient) { }

  // Räume Endpoints mit Error Handling
  getRaume(): Observable<Raum[]> {
    return this.http.get<Raum[]>(`${this.apiUrl}/raume`).pipe(
      catchError(this.handleError)
    );
  }

  getRaumById(id: number): Observable<Raum> {
    return this.http.get<Raum>(`${this.apiUrl}/raume/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Buchungen Endpoints mit Error Handling
  getBuchungen(): Observable<Buchung[]> {
    return this.http.get<Buchung[]>(`${this.apiUrl}/buchungen`).pipe(
      catchError(this.handleError)
    );
  }

  getBuchungById(id: number): Observable<Buchung> {
    return this.http.get<Buchung>(`${this.apiUrl}/buchungen/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  createBuchung(buchung: CreateBuchung): Observable<Buchung> {
    return this.http.post<Buchung>(`${this.apiUrl}/buchungen`, buchung).pipe(
      catchError(this.handleError)
    );
  }

  getBuchungenByRaum(raumId: number): Observable<Buchung[]> {
    return this.http.get<Buchung[]>(`${this.apiUrl}/buchungen/raum/${raumId}`).pipe(
      catchError(this.handleError)
    );
  }

  // Verfügbarkeit prüfen
  checkVerfuegbarkeit(raumId: number, startZeit: string, endZeit: string): Observable<boolean> {
    let params = new HttpParams()
      .set('raumId', raumId.toString())
      .set('startZeit', startZeit)
      .set('endZeit', endZeit);

    return this.http.get<boolean>(`${this.apiUrl}/buchungen/verfuegbarkeit`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  // Error Handling
 private handleError(error: HttpErrorResponse) {
  let errorMessage = 'Ein Fehler ist aufgetreten';
  
  if (error.error instanceof Error) {
    // Client-seitiger Fehler
    errorMessage = `Fehler: ${error.error.message}`;
  } else {
    // Server-seitiger Fehler
    errorMessage = `Server Fehler ${error.status}: ${error.message}`;
    if (error.error && typeof error.error === 'string') {
      errorMessage += ` - ${error.error}`;
    }
  }
  
  console.error('API Fehler:', error);
  return throwError(() => new Error(errorMessage));
}
}