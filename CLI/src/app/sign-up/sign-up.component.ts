import { Component } from '@angular/core';
import { FormControl, FormGroup, MinLengthValidator, Validators } from '@angular/forms';
import { ApiServiceService } from '../service/api-service.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  constructor(private apiService: ApiServiceService) { }

  signUpForm = new FormGroup({
    name: new FormControl('', [Validators.minLength(3), Validators.required]),
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', [Validators.minLength(6), Validators.required]),
    confirmPassword: new FormControl('', [Validators.minLength(6), Validators.required]),
    phone: new FormControl('', [Validators.minLength(10), Validators.maxLength(11), Validators.required])
  })

  checkPassword() {
    if (this.signUpForm.value.password !== this.signUpForm.value.confirmPassword) {
      alert('Passwords do not match');
    }
  }

  signUp() {
    this.checkPassword();
    if (this.signUpForm.valid) {
      this.apiService.createUser(this.signUpForm.value).subscribe((response) => {
        console.log(response);
        if (response && response.statusCode === 200) {
          alert('User created successfully');
        }
      });
    } else {
      alert('Form is invalid');
    }
  }
}
