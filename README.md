# ChainLedger

ChainLedger is a lightweight blockchain library for immutable data tracking, designed for C# applications. It provides an easy-to-use API for securely recording, validating, and querying task updates while ensuring data integrity using blockchain principles.

## Features
✅ Generic blockchain support (`BlockChain<T>`)  
✅ Proof-of-Work consensus mechanism  
✅ RSA digital signatures for security  
✅ Dependency Injection (DI) support  

## Usage
### Registering with Dependency Injection
```csharp
using Microsoft.Extensions.DependencyInjection;
using ChainLedger;

var services = new ServiceCollection();
services.AddChainLedger<string>(); // or any custom data type
```
### Adding a Task Update
```csharp
blockchainService.AddUpdate("Task 1");
```

### Retrieving Task History
```csharp
var history = blockchainService.GetHistory(b => b.Data.Value == "Task1");
```

### Validating the Blockchain
```csharp
bool isValid = blockchainService.ValidateBlockchain();
```

## Contributing
Contributions are welcome! Feel free to submit issues or pull requests.

## License
MIT License. See [LICENSE](LICENSE) for details.

## Author
Rabin
