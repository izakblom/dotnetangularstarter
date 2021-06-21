import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TickerConfiguratorComponent } from './ticker-configurator.component';

describe('TickerConfiguratorComponent', () => {
  let component: TickerConfiguratorComponent;
  let fixture: ComponentFixture<TickerConfiguratorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TickerConfiguratorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TickerConfiguratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
