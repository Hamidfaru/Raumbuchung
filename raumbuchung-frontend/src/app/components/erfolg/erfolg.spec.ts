import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Erfolg } from './erfolg';

describe('Erfolg', () => {
  let component: Erfolg;
  let fixture: ComponentFixture<Erfolg>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Erfolg]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Erfolg);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
