<script lang="ts">
    import "../app.css";
    import { onMount } from 'svelte';
    import { page } from '$app/stores';
    import { Navbar, NavBrand, NavLi, NavUl, NavHamburger, Dropdown, DropdownItem, Chevron, DropdownDivider } from 'flowbite-svelte';

    onMount(() => {
        document.documentElement.classList.add('dark');
        console.log($page.url.pathname);
    });
</script>

<Navbar let:hidden let:toggle color="none" navClass="px-2 sm:px-4 py-2.5 w-full bg-gray-800 border-b border-gray-700">
  <NavBrand href="/">
    <img
      src="/Signature-Vertical-M.png"
      class="mr-3 h-6 sm:h-9"
      alt="UofM Logo"
    />
    <span class="self-center whitespace-nowrap text-xl font-semibold dark:text-white">
      Quanta Track
    </span>
  </NavBrand>
  <NavHamburger on:click={toggle} />
  <NavUl {hidden} class="rounded-lg md:dark:bg-gray-900 dark:border-gray-700">
    <NavLi href="/" active={$page.url.pathname === "/"}>Home</NavLi>
    <NavLi id="nav-menu1" class="cursor-pointer" active={$page.url.pathname === "/receipt"}><Chevron aligned>Parcels</Chevron></NavLi>
    <NavLi href="/">Blocks & Slides</NavLi>
    <NavLi href="/">Kits</NavLi>
    <NavLi href="/">Contact</NavLi>
    <Dropdown triggeredBy="#nav-menu1" class="w-44 z-20">
      <DropdownItem href="/receipt">Receipt</DropdownItem>
      <DropdownItem>Opening</DropdownItem>
      <DropdownDivider />
      <DropdownItem>Lookup</DropdownItem>
    </Dropdown>
  </NavUl>
</Navbar>

<div class="bg-white dark:bg-gray-900 h-screen w-screen">
    <div class="mx-auto pt-10">
        <slot />
    </div>
  </div>