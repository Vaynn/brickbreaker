using Firebase;
using Firebase.Auth;
using System;

public class FirebaseErrorHandler
{
    /// <summary>
    /// Returns a user-friendly message from a Firebase AggregateException;
    /// </summary>
    public static string GetErrorMessage(AggregateException ex)
    {
        if (ex == null) return "Unknown error has occurred";

        var firebaseEx = ex.Flatten().InnerException as FirebaseException;
        if (firebaseEx != null)
        {
            switch ((AuthError)firebaseEx.ErrorCode)
            {
                case AuthError.InvalidEmail:
                    return "This Email is not valid.";
                case AuthError.EmailAlreadyInUse:
                    return "This email already exists.";
                case AuthError.WeakPassword:
                    return "Password too weak (min 6 characters).";
                case AuthError.WrongPassword:
                    return "Incorrect password.";
                case AuthError.UserNotFound:
                    return "This email correspond to any account .";
                case AuthError.UserDisabled:
                    return "This account is disabled .";
                default:
                    return firebaseEx.Message;
            }
        }

        return ex.Message;
    }
    
}
