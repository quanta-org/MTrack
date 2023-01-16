<script lang="ts">
    import { enhance } from '$app/forms';
    import { fade } from 'svelte/transition';
    import { Input, Label, Select, Button, Toast } from 'flowbite-svelte';
    import type { ActionData } from './$types';

    export let form: HTMLFormElement;
    var charArray: string[] = [];
    let ParcelReceipt = {
        ReceiverID: "John",
        ReceiptLocation: "",
        Carrier: "",
        TrackingNumber: "",
        RoutingLocation: ""
    }
    let success: boolean = false;
    let error: boolean = false;
    let toastMessage: String = "";
    let receiptLocations = [
        {value:"Location 1", name: "Location 1"},
        {value:"Location 2", name: "Location 2"},
        {value:"Location 3", name: "Location 3"},
        {value:"Location 4", name: "Location 4"},
    ]
    let routingLocations = [
        {value:"Location 1", name: "Location 1"},
        {value:"Location 2", name: "Location 2"},
        {value:"Location 3", name: "Location 3"},
        {value:"Location 4", name: "Location 4"},
    ]
    let couriers = [
        {value:"UPS", name:"UPS"},
        {value:"FedEx", name:"FedEx"},
        {value:"DHL", name:"DHL"},
        {value:"USPS", name:"USPS"},
        {value:"Other", name:"Other"},
    ]



    function onKeypress(event: any){
        if (event.key.length == 1){
            charArray.push(event.key);
        } else if (event.key == "Tab" && charArray.length >= 10){
            event.preventDefault();
            let tracknum = charArray.join("");

            if(ParcelReceipt.TrackingNumber === tracknum){
                form.submit();
                return;
            }

            let ups_regex = ["^(1Z)[0-9A-Z]{16}$", "^(T)+[0-9A-Z]{10}$", "^[0-9]{9}$", "^[0-9]{26}$"];
            let fedex_regex = ["^[0-9]{20}$", "^[0-9]{15}$", "^[0-9]{12}$", "^[0-9]{22}$"];
            let usps_regex = ["^(94|93|92|94|95)[0-9]{20}$", "^(94|93|92|94|95)[0-9]{22}$", "^(70|14|23|03)[0-9]{14}$", "^(M0|82)[0-9]{8}$", "^([A-Z]{2})[0-9]{9}([A-Z]{2})$"];

            //Detect UPS
            ups_regex.forEach(value => {
                if(tracknum.match(value)){
                    ParcelReceipt.Carrier = "UPS";
                }
            });

            //Detect FedEx
            fedex_regex.forEach(value => {
                if(tracknum.match(value)){
                    ParcelReceipt.Carrier = "FedEx";
                }
            });

            //Detect USPS
            usps_regex.forEach(value => {
                if(tracknum.match(value)){
                    ParcelReceipt.Carrier = "USPS";
                }
            });

            ParcelReceipt.TrackingNumber = tracknum;
            charArray = [];
        }
    }

    function clearArray(){
        charArray = [];
    }

    function sendToast(message: String = "", isError: boolean = false){
        toastMessage = message;
        if(isError){
            error = true;
            setTimeout(() => {
                error = false;
            }, 5000);
        } else {
            success = true;
            setTimeout(() => {
                success = false;
            }, 5000);
        }
    }

    /*
    async function postParcelReceipt(){
        // Form validation
        if(!ParcelReceipt.ReceiverID){
            sendToast("Invalid receiver", true);
            return;
        }
        if(!ParcelReceipt.ReceiptLocation){
            sendToast("Invalid receipt location", true);
            return;
        }
        if(!ParcelReceipt.Carrier){
            sendToast("Invalid carrier", true);
            return;
        }
        if(!ParcelReceipt.RoutingLocation){
            sendToast("Invalid routing location", true);
            return;
        }
        if(!ParcelReceipt.TrackingNumber){
            sendToast("Invalid tracking number", true);
            return;
        }

        // Post ParcelReceipt
        const response = await fetch("/api/ParcelReceipt", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(ParcelReceipt)
        });

        if (response.status === 200) {
            await invalidateAll();

            ParcelReceipt.TrackingNumber = "";

            sendToast("Successfully added parcel!");
        } else {
            sendToast(await response.text(), true);
        }
    }*/
</script>

<svelte:window on:keydown={onKeypress} on:mousemove={clearArray}></svelte:window>

<div class="flex justify-center">
    <Toast color="green" transition={fade} bind:open={success} class="mb-2 absolute">
        <svelte:fragment slot="icon">
          <svg aria-hidden="true" class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"></path></svg>
          <span class="sr-only">Check icon</span>
        </svelte:fragment>
        {toastMessage}
    </Toast>

    <Toast color="red" transition={fade} bind:open={error} class="mb-2 absolute">
        <svelte:fragment slot="icon">
          <svg aria-hidden="true" class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd"></path></svg>
          <span class="sr-only">Warning icon</span>
          </svelte:fragment>
        {toastMessage}
    </Toast>
</div>

<h1 class="text-4xl font-bold text-white mb-5 flex justify-center">Parcel Receipt</h1>
{#if ParcelReceipt.TrackingNumber}
    <h2 class="text-2xl text-white mb-5 flex justify-center">Scan again to add</h2>
{:else}
    <h2 class="text-2xl text-white mb-5 flex justify-center">Scan parcel or add tracking number</h2>
{/if}

<div class="flex justify-center">
	<form id="form" method="POST" class="w-96 p-5 bg-gray-800 rounded-xl" action="?/addParcelReceipt" bind:this={form} use:enhance={( {} ) => {
        return async ({ result, update }) => {
            if (result.type == 'success'){
                sendToast("Successfully added parcel!");
            } else if (result.type == 'failure'){
                sendToast(result.data?.message, true);
            }

            update();
        }
    }}>
        <div class="mb-6">
            <Label for="receiver" class="mb-2">Receiver</Label>
            <Input type="text" id="receiver" name="receiver" tabindex="-1" bind:value={ParcelReceipt.ReceiverID} on:change={clearArray} readonly required />
        </div>

        <div class="mb-6">
            <Label for="location" class="mb-2">Receipt Location</Label>
            <Select id="location" name="receiptLocation" tabindex="-1" items={receiptLocations} bind:value={ParcelReceipt.ReceiptLocation} class="mt-2" required />
        </div>

        <div class="mb-6">
            <Label for="carrier" class="mb-2">Carrier</Label>
            <Select id="carrier" name="carrier" tabindex="-1" items={couriers} bind:value={ParcelReceipt.Carrier} class="mb-2" required />
        </div>

        <div class="mb-6">
            <Label for="routlocation" class="mb-2">Routing Location</Label>
            <Select id="routlocation" name="routeLocation" tabindex="-1" items={routingLocations} bind:value={ParcelReceipt.RoutingLocation} class="mb-2" required />
        </div>

        <div class="mb-6">
            <Label for="tracknum" class="mb-2">Tracking Number</Label>
            <Input type="text" id="tracknum" name="trackNumber" tabindex="-1" bind:value={ParcelReceipt.TrackingNumber} placeholder="1Z 6F8..." required />
        </div>
        
        <div class="flex justify-center">
            <Button type="submit" tabindex="-1">Add Receipt</Button>
        </div>
    </form>
</div>

<style>
</style>
