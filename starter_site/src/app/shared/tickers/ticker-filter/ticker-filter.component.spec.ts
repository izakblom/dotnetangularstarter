import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TickerFilterComponent } from './ticker-filter.component';

describe('TickerFilterComponent', () => {
  let component: TickerFilterComponent;
  let fixture: ComponentFixture<TickerFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TickerFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TickerFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
