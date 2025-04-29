//1. Normal Hubs
//In a normal hub, method calls are made using string-based identifiers for client-side methods. This is the default way of using SignalR.

//## Characteristics
//• Flexible: You can dynamically invoke any method by its name.
//• Prone to Errors: Because method names and parameters are specified as strings, there's no compile-time checking, which can lead to runtime errors.
//• Simpler Setup: Easy to set up without requiring interfaces or additional abstractions.

//## Example of a Normal Hub
//public class ChatHub : Hub
//{
//    public async Task SendMessage(string user, string message)
//    {
//        await Clients.All.SendAsync("ReceiveMessage", user, message);
//    }
//}

//## Client-Side Code:
//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/chatHub")
//    .build();

//connection.start().then(() => {
//    console.log("Connected to the hub!");

//    // Invoke the hub method
//    connection.invoke("SendMessage", "User1", "Hello, World!");

//    // Listen for the "ReceiveMessage" event
//    connection.on("ReceiveMessage", (user, message) => {
//        console.log(`${user}: ${message}`);
//    });
//});

//# Pros of Normal Hubs
//• Quick and Easy: No need for extra setup.
//• Dynamic: Can handle dynamically named methods.

//# Cons of Normal Hubs
//• Error-Prone: Typos in method names or mismatched parameters aren't caught at compile time.
//• Harder to Maintain: Large projects can become challenging to debug or refactor.

//2. Strongly-Typed Hubs
//A strongly-typed hub uses an interface to define the methods that clients can call. This ensures that both the server and client adhere to a predefined contract, making the interaction type-safe.

//## Characteristics
//• Type-Safe: Ensures that method names and parameters match the interface, providing compile-time checking.
//• Maintainable: Easier to refactor and debug, especially in large projects.
//• Requires an Interface: An additional step to define the contract between the server and client.

//## Example of a Strongly-Typed Hub
//Define an Interface for Client-Side Methods:
//public interface IChatClient
//{
//    Task ReceiveMessage(string user, string message);
//}

//Implement the Strongly-Typed Hub:
//public class ChatHub : Hub<IChatClient>
//{
//    public async Task SendMessage(string user, string message)
//    {
//        await Clients.All.ReceiveMessage(user, message);
//    }
//}

//Client-Side Code: The client-side code doesn’t change much, but the server-side methods are now guaranteed to match the IChatClient interface.
//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/chatHub")
//    .build();

//connection.start().then(() => {
//    console.log("Connected to the hub!");

//    // Listen for the "ReceiveMessage" event
//    connection.on("ReceiveMessage", (user, message) => {
//        console.log(`${user}: ${message}`);
//    });
//});

//# Pros of Strongly-Typed Hubs
//• Compile-Time Safety: Method names and parameters are checked during compilation.
//• Improved Maintainability: Refactoring and debugging are easier with a defined contract.
//• Clear Contracts: The interface clearly defines what methods are available to the client.

//# Cons of Strongly-Typed Hubs
//• Additional Setup: Requires defining and maintaining an interface.
//• Less Flexible: Cannot dynamically invoke methods outside the defined interface.