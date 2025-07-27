import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainbackgroundComponent } from './mainbackground.component';

describe('MainbackgroundComponent', () => {
  let component: MainbackgroundComponent;
  let fixture: ComponentFixture<MainbackgroundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainbackgroundComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainbackgroundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
