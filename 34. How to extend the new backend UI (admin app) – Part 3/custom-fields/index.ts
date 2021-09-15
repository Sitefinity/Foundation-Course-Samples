import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { UserComponent } from "./user-picker/user-component";
import { UserPickerComponent } from "./user-picker/user-picker-write.component";
import { UserPickerReadonlyComponent } from "./user-picker/user-picker-readonly.component";
import { USER_PICKER_PROVIDER } from "./user-picker/user-picker-provider";
import { FrameworkModule } from "@progress/sitefinity-adminapp-sdk/app/api/v1";
import { HttpClientModule } from "@angular/common/http";
import { CommonModule } from "@angular/common";

/**
 * The custom fields module.
 */
@NgModule({
    declarations: [
        UserPickerComponent,
        UserPickerReadonlyComponent,
        UserComponent
    ],
    entryComponents: [
        // The components need to be registered here as they are instantiated dynamically.
        UserPickerComponent,
        UserPickerReadonlyComponent,
        UserComponent
    ],
    providers: [
        USER_PICKER_PROVIDER
    ],

    // import the framework module as it holds the components that the AdminApp uses
    // for a list of components see
    imports: [
        FormsModule, 
        FrameworkModule,
        HttpClientModule,
        CommonModule
    ]
})
export class CustomFieldsModule { }
