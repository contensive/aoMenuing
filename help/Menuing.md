
This add-on creates a dynamic menu as it is typically used on a bootstrap website. By default it uses a layout that creates the UL, LI, and A tags of the bootstrap Navbar. The designer may chose to create an alternate layout that modifies this is any way.

# How to add a bootstrap navbar to the page

Thie add-on is typically added directly into the html of the page template by the designer.

>    {% {"Navbar-Nav-Ul":{"instanceid":"Unique-Name-for-Menu"}} %}

This is a typical use of the menu in the template

>    <nav class="navbar navbar-expand-lg navbar-light bg-light">
>      <div class="container-fluid">
>        <a class="navbar-brand" href="#">Navbar</a>
>        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
>          <span class="navbar-toggler-icon"></span>
>        </button>
>        <div class="collapse navbar-collapse" id="navbarSupportedContent">
>          {% {"Navbar-Nav-Ul":{"instanceid":"Unique-Name-for-Menu"}} %}
>        </div>
>      </div>
>    </nav>

When it is first called, it creates a new menu record and automatically populates it with the default layout.

# To change the default layout

Make a copy of the default layout (Menuing Navbar-Nav-UL Default Layout) and select your copy in the menu record created for your instance of the menu.