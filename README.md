Visual Digital Seal Suite
=====================

Visible Digital Seal Suite provides services which produce a Visible Digital Seal for Non-Constrainted Environments (VDS-NC) for Proof of Vaccination (PoV).

Visible Digital Seal Suite is produced and maintained by the Australian Passport Office.

## Visual Digital Seal for Non-Constrained Environments

The VDS-NC is a cryptographically signed data structure containing information encoded as a 2D barcode. It can be printed on a document or issued in the form of a graphic for presentation using a device (e.g. smart phone). VDS-NC allows an individual to carry and present cryptographically verifiable Proof of Vaccination (PoV) or Proof of Test (PoT) for COVID-19 and other diseases/agents.

Visible Digital Seal Suite provides an implementation for the Proof of Vaccination (PoV) use case. Considerations have been made to support developers in implementing Proof of Test (PoT) or their own use cases while re-using as much common code as possible.

The VDS-NC standard is provided by the International Civil Aviation Authority (ICAO) (https://www.icao.int/).

### References

| Title | Description | Source |
| --- | --- | --- |
|ICAO VDS-NC Publication|ICAO Doc 9303 Part 13 defines a Visible Digital Seal for Non-Constrained environments. |https://www.icao.int/publications/Documents/9303_p13_cons_en.pdf|
|ICAO Doc 9303 Index|ICAO Doc 9303 provides standard and information for Machine Readable Travel Documents (MRTD). This includes VDS-NC as well as several standards which support it. |https://www.icao.int/publications/pages/publication.aspx?docnum=9303|

## Workflow Overview

Visible Digital Seal Suite defines a simple workflow which produces documents containing a VDS-NC barcode for Proof of Vaccination.

The workflow can be instantiated using dependency injection or construction.

The workflow provides:

1. A strongly typed `Vds<PovVdsMessage>` definition.
1. Validation of a VDS object before signing.
1. A pattern for signing VDS data.
1. A QR code renderer.
1. A Document Renderer.

The workflow produces a `VdsResult` containing:
1. A document/certificate.
2. A rendered barcode.
3. The VDS definition contained in the document.
4. A list of validation messages, if a fault occurs.

For more information on the workflow steps, see: [1.1 Workflow Overview](./Docs/1.1.WorkflowOverview.md)

In addition, the `Apo.VisibleDigitalSeal.Utility` namespace provides utilities for name truncation and check digits using algorithms defined by ICAO.

## Getting Started

This tutorial will allow you to produce a VDS-NC barcode as quickly as possible to demonstrate the output. 

After completing these steps, additional steps in [1.3 Next Steps](./Docs/1.3.0.NextSteps.md) will enable you to customize a production-ready VDS-NC for your organisation.

### 1. Startup.cs
Add the required services to your dependency injection container.

```csharp
using Apo.VisibleDigitalSeal.Extensions.DependencyInjection;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        ...

        services.AddVdsPreProcessorService();
        services.AddVdsValidationService();
        services.AddVdsStringEncodingService();
        services.AddVdsQrCodeService();
        services.AddVdsDemoSigningService_ForNonProduction();
        services.AddVdsBarcodeOnlyDocumentService();
        services.AddVdsWorkflow();
        
        ...
    }
}
```
> For a detailed example of individually registering or instantiating all services, see: [1.5 Service Registration](./Docs/1.5.ServiceRegistration.md).  
> This section also details the services to create if you are constructing services without dependency injection.

> **Note:** This example uses the `DemoSigningService`. This is provided to allow a test VDS-NC to be created while a signing service is designed. But cannot be used in production. More information on signing is provided after this example.

> **Note:** This example uses the `BarcodeOnlyDocumentService`. This will produce a functional VDS-NC barcode. Production systems will likely want to create a PDF document or certificate. More information on documents is provided after this example.


### 2. Example Api Controller
Add a controller with constructor injection for `IVisibleDigitalSealService<PovVdsMessage>`.

```csharp
using Apo.VisibleDigitalSeal;
using Apo.VisibleDigitalSeal.Models.Icao;

public class MyController : ControllerBase
{
    private readonly IVisibleDigitalSealGenerator<PovVdsMessage> _vdsService;

    public MyController(IVisibleDigitalSealGenerator<PovVdsMessage> vdsService)
    {
        _vdsService = vdsService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVds()
    {
        var vds = new Vds<PovVdsMessage> ()
        {
          // Populate VDS
        };

        // Create VDS Result
        var vdsResult = await _vdsGenerator
            .CreateVisibleDigitalSealDocument(vds);
        
        // Check result for errors
        if (vdsResult.ErrorMessages != null)
        {
            // A validation error occured
            return new JsonResult(vdsResult);
        }
        else
        {
            // Success. Return the document
            return File(vdsResult.Document.Bytes, "image/png", "MyVds.png");
        }
    }
}
```

## Next Steps

To prepare a production-ready VDS-NC workflow, you will need to implement an `ISigningService`. 

For information on where to start, see: [1.3 Next Steps](./Docs/1.3.0.NextSteps.md)  
For information on `ISigningService` and the provided `HttpSigningService`, see: [2.4 Signing Service](./Docs/2.4.SigningService.md)

You may also want to use a different `IDocumentService` ([2.6.0](./Docs/2.6.0.DocumentService.md)). Using the `HtmlDocumentService` ([2.6.2](./Docs/2.6.2.HtmlDocumentService.md)) or the `PdfDocumentService` ([2.6.3](./Docs/2.6.3.PdfDocumentService.md)) will allow you to embed the VDS-NC barcode into a document or certificate which provides branding or supporting information.

For information on where to start, see: [1.3 Next Steps](./Docs/1.3.0.NextSteps.md)  
For information on `IDocumentService` and its implementations, see: [2.6.0 Document Service](./Docs/2.6.0.DocumentService.md)
