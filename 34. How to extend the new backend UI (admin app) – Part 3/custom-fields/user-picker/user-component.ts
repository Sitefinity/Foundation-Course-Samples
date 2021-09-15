import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { HTTP_PREFIX } from "@progress/sitefinity-adminapp-sdk/app/api/v1";
import { User } from "./user";

@Component({
    templateUrl: "./user.component.html",
    styleUrls: [ "./user.component.css" ]
})

export class UserComponent implements OnInit{
    constructor(private http: HttpClient) { }

    public user: User | null;
    private userEndpoint = `${HTTP_PREFIX}/api/default/Default.GetUsers()`;

    users = Array<User>();

    ngOnInit(){
        this.http.get<any>(this.userEndpoint)
            .toPromise()
            .then(response => {
                this.users = this.getArray(JSON.parse(response.value[0].Data))
            });
    }

    changeSelection(selectedUser: User){
        selectedUser.Selected = !selectedUser.Selected;
        if(selectedUser.Selected){
            this.user = selectedUser;
        } else {
            this.user = null;
        }

        this.users = this.getArray(this.users);
    }

    getArray(users: User[]){
        return users.map(user => {
            return {
                ...user,
                Selected: this.user 
                    ? user.Id === this.user.Id
                    : false
            }
        }).sort((a, b ) => {
            if(a.Email.toLowerCase() < b.Email.toLowerCase()){
                return -1;
            }
            if(a.Email.toLowerCase() > b.Email.toLowerCase()){
                return 1;
            }
            return 0;
        });
    }
}