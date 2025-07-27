import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NEWSLETTERComponent } from './newsletter.component';

describe('NEWSLETTERComponent', () => {
  let component: NEWSLETTERComponent;
  let fixture: ComponentFixture<NEWSLETTERComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NEWSLETTERComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NEWSLETTERComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
