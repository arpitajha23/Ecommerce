import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgeturlpageComponent } from './forgeturlpage.component';

describe('ForgeturlpageComponent', () => {
  let component: ForgeturlpageComponent;
  let fixture: ComponentFixture<ForgeturlpageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ForgeturlpageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForgeturlpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
