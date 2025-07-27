import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressesPageComponent } from './addresses-page.component';

describe('AddressesPageComponent', () => {
  let component: AddressesPageComponent;
  let fixture: ComponentFixture<AddressesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddressesPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddressesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
