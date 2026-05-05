import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDividerModule,
    MatIconModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  currencies: string[] = ['USD', 'EUR', 'GBP', 'INR', 'JPY', 'CAD', 'AUD'];

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.profileForm = this.fb.group({
      firstName: [{ value: '', disabled: true }],
      lastName: [{ value: '', disabled: true }],
      email: [{ value: '', disabled: true }],
      preferredCurrency: ['USD', Validators.required]
    });
  }

  ngOnInit(): void {
    const user = this.authService.getUser();
    if (user) {
      this.profileForm.patchValue({
        firstName: user.firstName || '',
        lastName: user.lastName || '',
        email: user.email || ''
      });
    }

    const savedCurrency = localStorage.getItem('preferredCurrency');
    if (savedCurrency) {
      this.profileForm.patchValue({ preferredCurrency: savedCurrency });
    }
  }

  saveSettings(): void {
    if (this.profileForm.valid) {
      const currency = this.profileForm.get('preferredCurrency')?.value;
      localStorage.setItem('preferredCurrency', currency);
      Swal.fire('Success', 'Preferences saved successfully!', 'success');
    }
  }
}
