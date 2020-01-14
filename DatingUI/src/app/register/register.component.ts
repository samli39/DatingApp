import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from 'src/services/auth/auth.service';
import { AlertifyService } from 'src/services/alertify/alertify.service';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  model: any = {}
  @Output() cancelRegister = new EventEmitter();
  constructor(private auth: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    this.auth.register(this.model).subscribe(data => {
      this.alertify.success("registration done");
    }, err => {
      this.alertify.error(err);
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
