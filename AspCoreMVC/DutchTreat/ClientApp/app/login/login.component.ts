import { Component } from "@angular/core";
import { Router } from "@angular/router"
import { DataService } from '../shared/dataService';


@Component({
  selector: "login",
    templateUrl: "login.component.html",
    styleUrls: ['login.component.css']
})
export class Login {

    constructor(private data: DataService, private router: Router) {
  }
  
    public creds = {
        username: "",
        password:""
    }
    public errorMessage: string;
    onLogin() {
        //Call the login Service
        this.data.login(this.creds)
            .subscribe(success => {
                if (this.data.order.items.length == 0) {
                    this.router.navigate([""]);
                } else {
                    this.router.navigate(["checkout"]);
                }
            }, err => this.errorMessage = "Login failed");
    }
}