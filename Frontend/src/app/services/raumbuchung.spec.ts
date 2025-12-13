import { TestBed } from '@angular/core/testing';

import { Raumbuchung } from './raumbuchung';

describe('Raumbuchung', () => {
  let service: Raumbuchung;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Raumbuchung);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
