import { type IAccountService, type ProfileResponse } from "../types/AccountTypes"

const accountService: IAccountService = {
    getProfile: function (): Promise<ProfileResponse> {
        throw new Error("Function not implemented.")
    }
}