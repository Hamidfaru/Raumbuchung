import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RaumList } from './raum-list';

describe('RaumList', () => {
  let component: RaumList;
  let fixture: ComponentFixture<RaumList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RaumList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RaumList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
