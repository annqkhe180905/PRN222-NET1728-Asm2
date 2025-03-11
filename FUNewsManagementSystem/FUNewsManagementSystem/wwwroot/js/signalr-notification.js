document.addEventListener("DOMContentLoaded", function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/crudHub") // Ensure this matches your backend SignalR Hub URL
        .configureLogging(signalR.LogLevel.None) // Disable logs if not needed
        .build();

    connection.start().catch(err => console.error("SignalR Connection Error: ", err.toString()));

    // Prevent multiple reloads by setting a flag
    let hasReloaded = false;

    function refreshPage() {
        if (!hasReloaded) {
            hasReloaded = true; // Set flag to prevent multiple reloads
            setTimeout(() => {
                location.reload(); // Refresh the page
            }, 500); // Short delay to ensure smooth update
        }
    }

    // Reload the page only when Create, Update, or Delete occurs
    connection.on("EntityCreated", refreshPage);
    connection.on("EntityUpdated", refreshPage);
    connection.on("EntityDeleted", refreshPage);
});
