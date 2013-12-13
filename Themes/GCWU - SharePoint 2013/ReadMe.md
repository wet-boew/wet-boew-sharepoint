/* This template is still in Beta */

Information
===================
In SharePoint 2013, master pages are now plain HTML files that contain special pieces of codes named SharePoint Snippets. When uploaded to your Master Page gallery, the HTML master page automatically creates an associated .master file. 

The current solution contains two files, the HTML master page based off the WET 3.1.6 Internet Template, and a .wsp sandbox solution. The sandbox solution contains all the WET artefacts (images, css, and js) that are required by the master page to render properly.

Contacts
===================
This GitHub project is provided by Nik Charlebois. If you have any questions or comments, or if you are interested in helping with the development, please contact me at

Nikolas.Charlebois-Laprade@BAC-LAC.gc.ca

You can also reach me on twitter (@NikCharlebois), on facebook, on linked in, by smoke signals, carrier pidgeons, etc

Installation
===================
Sandbox Solution
-------------------
1 - Navigate to your solutions gallery (http://<your domain>/_catalogs/solution/)

2 - Upload the SPWetResources.wsp file;

3 - Activate the SPWetResources solution;

4 - Navigate to http://[your domain]/style library/css style sheets/ and ensure there is an existing GCWU folder in there;

4.a - If the folder exists, you're golden;

4.b - If the folder is not there, go in the fridge, grab a beer and start back at step 1, you've done something wrong;

HTML Master Page
--------------------
1 - Make sure the publishing infrastructure feature is enabled on the site collection;

2 - Navigate to the Design Manager Administration of your site collection (http://[your domain]/_layouts/15/DesignWelcomePage.aspx)

3 - In the left menu, click on Edit Master Pages

4 - In the content area, click on "Convert an HTML file to a SharePoint master page"

5 - At the top of the dialog box, click on "add" in the sentence "Click to add new item"

6 - Browse to the HTML master page file, and upload it;

7 - Click "Insert"  to go back to the Edit Master page page;

8 - In the left menu, click on item 7 "Publish and Apply Design";

9 - In the content area, click on "Assign master pages to your site based on device channel";

10 - In the site Master Page section, in the drop down, pick the newly uploaded master page;

11 - Click "OK"

12 - Navigate back to your main page and ensure the new template is applied;

12a. If the new template is showing properly, you're done;

12b. If the new template doesn't show, go to step 4b of the previous section ;)