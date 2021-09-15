import { Component, Injectable, Inject, OnInit } from "@angular/core";
import { FieldBase, SelectorService, SELECTOR_SERVICE } from "@progress/sitefinity-adminapp-sdk/app/api/v1";
import { UserComponent } from './user-component';
import { User } from "./user";

@Component({
    templateUrl: "./user-picker-write.component.html",
    styleUrls: [ "./user-picker-write.component.css" ]
})
@Injectable()
export class UserPickerComponent extends FieldBase implements OnInit{
    constructor(@Inject(SELECTOR_SERVICE) private readonly selectorService: SelectorService) { 
        super();
     }
     user: User | null;

    get parsedValue(){
        if(this.getValue()){
            return JSON.parse(this.getValue())
        }

        return null;
    }

    ngOnInit(){
        this.user = this.parsedValue;
    }

    openDialog(){
        this.user = this.parsedValue;
        const userComponent = {
            componentData:{
                type: UserComponent,
                properties:{
                    user: this.user
                }
            },
            commands: [{
                name: "Cancel",
                title: "Cancel",
                ordinal: -1,
                category: "cancel"
            },
            {
                name: "Confirm",
                title: "Confirm",
                ordinal: -1,
                category: "primary"
            }]
        }

        this.selectorService.openDialog(userComponent)
            .subscribe(response => {
                if(response.data.data){
                    if(response.component.user){
                        this.user = response.component.user;
                        this.writeValue(JSON.stringify(this.user));
                    } else { 
                        this.clearUser();
                    }
                }
            });
    }

    clearUser(){
        this.user = null;
        this.writeValue(null);
    }
}
