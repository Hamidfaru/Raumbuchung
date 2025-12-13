import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuchungForm } from './buchung-form';

describe('BuchungForm', () => {
  let component: BuchungForm;
  let fixture: ComponentFixture<BuchungForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BuchungForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuchungForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
