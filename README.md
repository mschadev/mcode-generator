# mcode-generator
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
[![](https://img.shields.io/badge/readme%20style-standard-green)](https://github.com/RichardLitt/standard-readme)
![](https://img.shields.io/github/repo-size/zxc010613/mcode-generator)  
Online machine code(MCode) generator for autohotkey
## Notice
Domain([mcode-generator.com](http://mcode-generator.com/) has expired, so online services are no longer available.
## Table-of Contents
- [Install](#Install)
    - [dependencies](#dependencies)
- [Usage](#Usage)
  - [Required Environment Variables](#Required-Environment-Variables)
  - [Native](#Native)
  - [Docker](#Docker)
- [Maintainer](#Maintainer)
- [Contributing](#Contributing)
    - [Contributors](#Contributors)
- [License](#License)

## Install

### dependencies
If __Native__ Environment
1. bash
2. .NET core
3. gcc

If __Docker__ Environment
1. bash
2. Docker

```bash
git clone https://github.com/zxc010613/mcode-generator.git
```
## Usage
### Required Environment Variables
|Name|Description|
-----|-----------|
|PUBLISHER_NAME|Name of publisher|
|PUBLISHER_EMAIL|Email of publisher|
|GITUSER_URL|Your Git Profile URL|
### Native
```bash
cd mcode-generator
dotnet run
```

### Docker
- Image Build
```bash
docker build --force-rm=false --no-cache=false --rm=true -t mcode-generator:latest .
```
- Run
```bash
docker run -it --rm -p 5000:5000 -e ASPNETCORE_URLS=http://+:5000 --name mcode-generator1 mcode-generator:latest
```
## Maintainer
[@zxc010613](https://github.com/zxc010613)

## Contributing
No Restrictions. Open the Issue or Submit a PRs!  

### Contributors
Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):  
<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="http://plorence.kr/"><img src="https://avatars3.githubusercontent.com/u/29756197?v=4" width="100px;" alt=""/><br /><sub><b>Plorence</b></sub></a><br /><a href="https://github.com/zxc010613/mcode-generator/commits?author=zxc010613" title="Code">💻</a></td>
    <td align="center"><a href="https://joedf.ahkscript.org"><img src="https://avatars2.githubusercontent.com/u/3848219?v=4" width="100px;" alt=""/><br /><sub><b>Joe DF</b></sub></a><br /><a href="https://github.com/zxc010613/mcode-generator/commits?author=joedf" title="Code">💻</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->
<!-- ALL-CONTRIBUTORS-LIST:END -->
This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
## License
[MIT](./LICENSE)
