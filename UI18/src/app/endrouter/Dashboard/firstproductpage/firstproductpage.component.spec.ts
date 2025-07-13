import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FirstproductpageComponent } from './firstproductpage.component';

describe('FirstproductpageComponent', () => {
  let component: FirstproductpageComponent;
  let fixture: ComponentFixture<FirstproductpageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FirstproductpageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FirstproductpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
