import { ComponentFixture, TestBed } from '@angular/core/testing';

import {RaumListComponent } from './raum-list.component';

describe('RaumListComponent', () => {
  let component: RaumListComponent;
  let fixture: ComponentFixture<RaumListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RaumListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RaumListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
