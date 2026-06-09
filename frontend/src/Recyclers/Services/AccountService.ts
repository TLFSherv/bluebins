import { type IAccountService, type ProfileResponse } from "../Types/AccountTypes"

const accountService: IAccountService = {
    getProfile: function (): Promise<ProfileResponse> {
        throw new Error("Function not implemented.")
    }
}